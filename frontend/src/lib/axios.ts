import Axios from 'axios'
const axios = Axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'
})

// 401 interceptor -> logout
import { useAuth } from '../context/AuthContext'
export default axios

// hook to attach 401 handling in components
export function useAxiosAuth() {
  const { logout } = useAuth()
  React.useEffect(() => {
    const id = axios.interceptors.response.use(
      r => r,
      err => {
        if (err.response?.status === 401) logout()
        return Promise.reject(err)
      })
    return () => axios.interceptors.response.eject(id)
  }, [logout])
  return axios
}
