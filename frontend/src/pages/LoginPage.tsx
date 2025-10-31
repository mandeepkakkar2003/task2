import { useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../lib/api'
import { useAuth } from '../context/AuthContext'

export default function LoginPage() {
  const { login } = useAuth()
  const [email,setEmail]=useState(''); const [password,setPassword]=useState('')
  const [loading,setLoading]=useState(false); const [err,setErr]=useState<string|null>(null)

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault()
    setErr(null); setLoading(true)
    try {
      const { token } = await api.login(email, password)
      login(token, '')
    } catch (e:any) {
      setErr(e.response?.data?.detail || 'Login failed')
    } finally { setLoading(false) }
  }

  return (
    <div className="mx-auto max-w-md p-6">
      <div className="card">
        <h1 className="text-xl font-semibold mb-4">Login</h1>
        {err && <p className="text-red-600 mb-2">{err}</p>}
        <form onSubmit={onSubmit} className="space-y-3">
          <label className="block">
            <span className="text-sm">Email</span>
            <input className="input" type="email" value={email} onChange={e=>setEmail(e.target.value)} required />
          </label>
          <label className="block">
            <span className="text-sm">Password</span>
            <input className="input" type="password" value={password} onChange={e=>setPassword(e.target.value)} required minLength={6}/>
          </label>
          <button className="btn w-full" disabled={loading}>{loading?'Logging in...':'Login'}</button>
        </form>
        <p className="text-sm mt-3">No account? <Link className="underline" to="/register">Register</Link></p>
      </div>
    </div>
  )
}
