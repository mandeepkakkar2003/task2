import React, { createContext, useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import axios from '../lib/axios'

type AuthCtx = {
  token: string | null
  login: (t: string, exp: string) => void
  logout: () => void
}
const Ctx = createContext<AuthCtx | null>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'))
  const navigate = useNavigate()

  useEffect(() => {
    if (token) axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
    else delete axios.defaults.headers.common['Authorization']
  }, [token])

  const login = (t: string) => {
    setToken(t)
    localStorage.setItem('token', t)
    navigate('/')
  }
  const logout = () => {
    setToken(null)
    localStorage.removeItem('token')
    navigate('/login')
  }

  return <Ctx.Provider value={{ token, login, logout }}>{children}</Ctx.Provider>
}

export function useAuth() {
  const ctx = useContext(Ctx)
  if (!ctx) throw new Error('AuthContext missing')
  return ctx
}
