export interface Superpower {
  id: number
  superpower: string
  description: string
}

export interface Hero {
  id: number
  name: string
  heroName: string
  birthDate: string
  height: number
  weight: number
  createdAt: string
  updatedAt: string
}

export interface HeroDetail extends Hero {
  superpowers: Superpower[]
}

export interface CreateHeroRequest {
  name: string
  heroName: string
  birthDate: string
  height: number
  weight: number
  superpowerIds: number[]
}

export interface UpdateHeroRequest {
  name: string
  heroName: string
  birthDate: string
  height: number
  weight: number
  superpowerIds: number[]
}

export interface ApiResponse<T> {
  data: T
  traceId: string
  executionTime: string
  statusCode: number
  success: boolean
  failure: boolean
  notifications: Notification[]
}
