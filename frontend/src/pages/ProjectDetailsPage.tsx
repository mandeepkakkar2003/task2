import { useEffect, useMemo, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { api, Task } from '../lib/api'

export default function ProjectDetailsPage() {
  const { id } = useParams()
  const projectId = id!
  const [project, setProject] = useState<{title:string, description?:string} | null>(null)
  const [tasks, setTasks] = useState<Task[]>([])
  const [title,setTitle]=useState(''); const [due,setDue]=useState('')
  const [loading,setLoading]=useState(false); const [err,setErr]=useState<string|null>(null)
  const [sched,setSched]=useState<string[]|null>(null)

  async function load() {
    try {
      const [p, t] = await Promise.all([api.getProject(projectId), api.listTasks(projectId)])
      setProject(p); setTasks(t)
    } catch(e:any) { setErr(e.response?.data?.detail || 'Failed to load') }
  }
  useEffect(()=>{ load() },[projectId])

  async function addTask(e: React.FormEvent) {
    e.preventDefault(); if(title.trim().length<1){ setErr('Task title required'); return }
    setLoading(true); setErr(null)
    try { await api.createTask(projectId, title, due || undefined); setTitle(''); setDue(''); await load() }
    catch(e:any){ setErr(e.response?.data?.detail||'Failed to add') }
    finally { setLoading(false) }
  }

  async function toggle(t: Task) {
    await api.updateTask(t.id, { title: t.title, dueDate: t.dueDate, isCompleted: !t.isCompleted })
    await load()
  }
  async function update(t: Task, newTitle: string) {
    await api.updateTask(t.id, { title: newTitle, dueDate: t.dueDate, isCompleted: t.isCompleted })
    await load()
  }
  async function del(id:string){ await api.deleteTask(id); await load() }

  const schedulePayload = useMemo(()=> tasks.map(t => ({
    title: t.title, estimatedHours: 1, dueDate: t.dueDate, dependencies: [] as string[]
  })), [tasks])

  async function runSchedule() {
    try {
      const { recommendedOrder } = await api.schedule(projectId, schedulePayload)
      setSched(recommendedOrder)
    } catch(e:any){
      setSched(null)
      setErr(e.response?.data?.detail || 'Schedule failed')
    }
  }

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">{project?.title || 'Project'}</h1>
        <Link className="underline" to="/">Back</Link>
      </div>
      {project?.description && <p className="text-gray-700">{project.description}</p>}
      {err && <p className="text-red-600">{err}</p>}

      <form onSubmit={addTask} className="card space-y-3">
        <input className="input" placeholder="Task title (1–200)" value={title} onChange={e=>setTitle(e.target.value)} maxLength={200}/>
        <input className="input" type="datetime-local" value={due} onChange={e=>setDue(e.target.value)} />
        <button className="btn" disabled={loading}>{loading?'Adding...':'Add Task'}</button>
      </form>

      <div className="card">
        <h2 className="font-medium mb-3">Tasks</h2>
        <ul className="space-y-2">
          {tasks.map(t=>(
            <li key={t.id} className="flex items-center gap-2">
              <input type="checkbox" checked={t.isCompleted} onChange={()=>toggle(t)} />
              <input className="input flex-1" value={t.title} onChange={e=>update(t, e.target.value)} />
              <button className="btn" onClick={()=>del(t.id)}>Delete</button>
            </li>
          ))}
          {tasks.length===0 && <p className="text-sm text-gray-600">No tasks yet.</p>}
        </ul>
      </div>

      <div className="card">
        <div className="flex items-center justify-between">
          <h2 className="font-medium">Smart Scheduler</h2>
          <button className="btn" onClick={runSchedule}>Run</button>
        </div>
        {sched && <ol className="list-decimal ml-6 mt-3">{sched.map(s=><li key={s}>{s}</li>)}</ol>}
      </div>
    </div>
  )
}
