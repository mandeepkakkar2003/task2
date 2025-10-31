import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api, Project } from '../lib/api'
import { useAuth } from '../context/AuthContext'

export default function DashboardPage() {
  const { logout } = useAuth()
  const [projects, setProjects] = useState<Project[]>([])
  const [title, setTitle] = useState(''); const [desc, setDesc]=useState('')
  const [loading, setLoading]=useState(false); const [err,setErr]=useState<string|null>(null)

  async function refresh() {
    try { setProjects(await api.getProjects()) } catch(e:any){ setErr(e.response?.data?.detail||'Failed to load') }
  }
  useEffect(() => { refresh() }, [])

  async function create(e: React.FormEvent) {
    e.preventDefault(); if (title.trim().length<3) { setErr('Title must be at least 3'); return }
    setLoading(true); setErr(null)
    try { await api.createProject(title, desc || undefined); setTitle(''); setDesc(''); await refresh() }
    catch(e:any){ setErr(e.response?.data?.detail||'Failed to create') }
    finally { setLoading(false) }
  }

  async function del(id:string){ if(!confirm('Delete project?'))return; await api.deleteProject(id); await refresh() }

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">Your Projects</h1>
        <button className="text-sm underline" onClick={logout}>Logout</button>
      </div>

      <form onSubmit={create} className="card space-y-3">
        {err && <p className="text-red-600">{err}</p>}
        <input className="input" placeholder="Project title (3–100)" value={title} onChange={e=>setTitle(e.target.value)} maxLength={100}/>
        <input className="input" placeholder="Description (optional, ≤500)" value={desc} onChange={e=>setDesc(e.target.value)} maxLength={500}/>
        <button className="btn" disabled={loading}>{loading?'Creating...':'Create Project'}</button>
      </form>

      <div className="grid gap-3">
        {projects.length===0 && <p className="text-sm text-gray-600">No projects yet.</p>}
        {projects.map(p=>(
          <div key={p.id} className="card flex items-center justify-between">
            <div>
              <Link className="underline font-medium" to={`/projects/${p.id}`}>{p.title}</Link>
              {p.description && <p className="text-sm text-gray-600">{p.description}</p>}
              <p className="text-xs text-gray-500">Created {new Date(p.createdAt).toLocaleString()}</p>
            </div>
            <button className="btn" onClick={()=>del(p.id)}>Delete</button>
          </div>
        ))}
      </div>
    </div>
  )
}
