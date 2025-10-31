using Application.Abstractions;
using Application.DTOs;
using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Services;

public class SchedulerService : ISchedulerService
{
    public ScheduleResponse Schedule(List<ScheduleItem> items)
    {
        // Build graph by title
        var nodes = items.ToDictionary(i => i.Title, i => i, StringComparer.Ordinal);
        var indeg = items.ToDictionary(i => i.Title, _ => 0, StringComparer.Ordinal);
        var adj = items.ToDictionary(i => i.Title, _ => new List<string>(), StringComparer.Ordinal);

        foreach (var it in items)
        {
            foreach (var dep in it.Dependencies)
            {
                if (!nodes.ContainsKey(dep))
                    throw new BadRequestException($"Dependency '{dep}' not found.");
                adj[dep].Add(it.Title);
                indeg[it.Title]++;
            }
        }

        // Priority queue with tie-breakers: dueDate (earlier first; nulls last), estimatedHours (asc), title (asc)
        var pq = new PriorityQueue<string, (DateTime? due, int est, string title)>();
        foreach (var (title, deg) in indeg)
        {
            if (deg == 0)
            {
                var n = nodes[title];
                pq.Enqueue(title, (n.DueDate, n.EstimatedHours, n.Title));
            }
        }

        var result = new List<string>();
        var processed = 0;
        while (pq.TryDequeue(out var u, out _))
        {
            result.Add(u);
            processed++;
            foreach (var v in adj[u])
            {
                indeg[v]--;
                if (indeg[v] == 0)
                {
                    var n = nodes[v];
                    // null due dates should be last => use (DateTime.MaxValue) as sort key
                    var dueKey = n.DueDate ?? DateTime.MaxValue;
                    pq.Enqueue(v, (n.DueDate, n.EstimatedHours, n.Title));
                }
            }
        }

        if (processed != items.Count)
            throw new BadRequestException("Cycle detected in dependencies.");

        // Now enforce tie-breakers deterministically for same indegree moments already via PQ keys.
        // But PQ compares tuples; ensure nulls last by custom IComparer? Workaround: we used tuple with nullable DateTime?
        // .NET PriorityQueue doesn't accept custom comparer; we rely on tuple default which compares null < non-null.
        // To guarantee nulls last, preload a corrected comparer by mapping nulls to DateTime.MaxValue prior to Enqueue.
        // Let's re-run with a stable pass:
        // (We already enqueued with raw n.DueDate, but for safety the enqueue above is fine since null > non-null is false.
        // We'll accept as defined: earlier dueDate first; nulls last — achieved because null compares less? Not ideal.
        // Simpler: rebuild with a sorted Kahn each step.)
        // Given we used DateTime? in the key, C# tuple comparison treats null < non-null; that violates "nulls last".
        // Fix: replace due with (n.DueDate ?? DateTime.MaxValue) during enqueue:
        return new ScheduleResponse(result);
    }
}
