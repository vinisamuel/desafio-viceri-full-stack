import api from './api'
import type { ApiResponse, Hero, HeroDetail, CreateHeroRequest, UpdateHeroRequest } from '../types'

export const heroService = {
  async getAll(): Promise<Hero[]> {
    const response = await api.get<ApiResponse<Hero[]>>('/heroes')
    return response.data.data
  },

  async getById(id: number): Promise<HeroDetail> {
    const response = await api.get<ApiResponse<HeroDetail>>(`/heroes/${id}`)
    return response.data.data
  },

  async create(hero: CreateHeroRequest): Promise<HeroDetail> {
    const response = await api.post<ApiResponse<HeroDetail>>('/heroes', hero)
    return response.data.data
  },

  async update(id: number, hero: UpdateHeroRequest): Promise<HeroDetail> {
    const response = await api.put<ApiResponse<HeroDetail>>(`/heroes/${id}`, hero)
    return response.data.data
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/heroes/${id}`)
  },
}
