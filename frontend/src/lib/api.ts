 import axios from './axios'

export type Project = { id: string, title: string, description?: string, createdAt: string }
export type Task = { id: string, projectId: string, title: string, dueDate?: string, isCompleted: boolean }

export const api = {
  // Auth
  async login(email: string, password: string) {
    const { data } = await axios.post<{ token: string, expiresAt: string }>('/api/auth/login', { email, password })
    return data
  },
  async register(email: string, password: string) {
    await axios.post('/api/auth/register', { email, password })
  },

  // Projects
  async getProjects() {
    const { data } = await axios.get<Project[]>('/api/projects')
    return data
  },
  async createProject(title: string, description?: string) {
    const { data } = await axios.post<Project>('/api/projects', { title, description })
    return data
  },
  async getProject(id: string) {
    const { data } = await axios.get<Project>(`/api/projects/${id}`)
    return data
  },
  async deleteProject(id: string) {
    await axios.delete(`/api/projects/${id}`)
  },

  // Tasks
  async listTasks(projectId: string) {
    const { data } = await axios.get<Task[]>(`/api/projects/${projectId}/tasks`)
    return data
  },
  async createTask(projectId: string, title: string, dueDate?: string) {
    const { data } = await axios.post<Task>(`/api/projects/${projectId}/tasks`, { title, dueDate })
    return data
  },
  async updateTask(taskId: string, payload: { title: string, dueDate?: string, isCompleted: boolean }) {
    const { data } = await axios.put<Task>(`/api/tasks/${taskId}`, payload)
    return data
  },
  async deleteTask(taskId: string) {
    await axios.delete(`/api/tasks/${taskId}`)
  },

  // Scheduler
  async schedule(projectId: string, items: { title: string, estimatedHours: number, dueDate?: string, dependencies: string[] }[]) {
    const { data } = await axios.post<{ recommendedOrder: string[] }>(`/api/v1/projects/${projectId}/schedule`, items)
    return data
  }
}
