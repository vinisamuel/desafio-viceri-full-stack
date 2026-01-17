import { defineStore } from 'pinia'
import { ref } from 'vue'
import { heroService } from '../services/heroService'
import type { Hero, HeroDetail, CreateHeroRequest, UpdateHeroRequest } from '../types'

export const useHeroStore = defineStore('hero', () => {
  const heroes = ref<Hero[]>([])
  const currentHero = ref<HeroDetail | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchHeroes() {
    loading.value = true
    error.value = null
    try {
      heroes.value = await heroService.getAll()
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to load heroes'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchHeroById(id: number) {
    loading.value = true
    error.value = null
    try {
      currentHero.value = await heroService.getById(id)
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to load hero'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createHero(hero: CreateHeroRequest) {
    loading.value = true
    error.value = null
    try {
      const newHero = await heroService.create(hero)
      await fetchHeroes()
      return newHero
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to create hero'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateHero(id: number, hero: UpdateHeroRequest) {
    loading.value = true
    error.value = null
    try {
      const updatedHero = await heroService.update(id, hero)
      await fetchHeroes()
      return updatedHero
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to update hero'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteHero(id: number) {
    loading.value = true
    error.value = null
    try {
      await heroService.delete(id)
      await fetchHeroes()
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to delete hero'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    heroes,
    currentHero,
    loading,
    error,
    fetchHeroes,
    fetchHeroById,
    createHero,
    updateHero,
    deleteHero,
  }
})
