import { defineStore } from 'pinia'
import { ref } from 'vue'
import { superpowerService } from '../services/superpowerService'
import type { Superpower } from '../types'

export const useSuperpowerStore = defineStore('superpower', () => {
  const superpowers = ref<Superpower[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchSuperpowers() {
    loading.value = true
    error.value = null
    try {
      superpowers.value = await superpowerService.getAll()
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to load superpowers'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    superpowers,
    loading,
    error,
    fetchSuperpowers,
  }
})
