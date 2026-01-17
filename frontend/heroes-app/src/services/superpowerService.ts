import api from './api'
import type { ApiResponse, Superpower } from '../types'

export const superpowerService = {
  async getAll(): Promise<Superpower[]> {
    const response = await api.get<ApiResponse<Superpower[]>>('/superpowers')
    return response.data.data
  },
}
