<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useHeroStore } from '../stores/heroStore'
import { useSuperpowerStore } from '../stores/superpowerStore'
import type { CreateHeroRequest } from '../types'
import '../assets/styles/views/HeroForm.css'

import Card from 'primevue/card'
import InputText from 'primevue/inputtext'
import Calendar from 'primevue/calendar'
import InputNumber from 'primevue/inputnumber'
import MultiSelect from 'primevue/multiselect'
import Button from 'primevue/button'
import { useNotification } from '../composables/useNotification'

const router = useRouter()
const route = useRoute()
const heroStore = useHeroStore()
const superpowerStore = useSuperpowerStore()
const notification = useNotification()

const isEditMode = computed(() => !!route.params.id)
const heroId = computed(() => Number(route.params.id))

const formData = ref<CreateHeroRequest>({
  name: '',
  heroName: '',
  birthDate: '',
  height: 0,
  weight: 0,
  superpowerIds: []
})

const errors = ref<Record<string, string>>({})
const loading = ref(false)
const selectedDate = ref<Date | null>(null)

onMounted(async () => {
  await superpowerStore.fetchSuperpowers()
  
  if (isEditMode.value) {
    await heroStore.fetchHeroById(heroId.value)
    if (heroStore.currentHero) {
      formData.value = {
        name: heroStore.currentHero.name,
        heroName: heroStore.currentHero.heroName,
        birthDate: heroStore.currentHero.birthDate,
        height: heroStore.currentHero.height,
        weight: heroStore.currentHero.weight,
        superpowerIds: heroStore.currentHero.superpowers.map(sp => sp.id)
      }
      selectedDate.value = new Date(heroStore.currentHero.birthDate)
    }
  }
})

function validateForm(): boolean {
  errors.value = {}
  
  if (!formData.value.name?.trim()) {
    errors.value.name = 'Name is required'
  }
  
  if (!formData.value.heroName?.trim()) {
    errors.value.heroName = 'Hero name is required'
  }
  
  if (!selectedDate.value) {
    errors.value.birthDate = 'Birth date is required'
  } else if (selectedDate.value > new Date()) {
    errors.value.birthDate = 'Birth date cannot be in the future'
  }
  
  if (!formData.value.height || formData.value.height <= 0) {
    errors.value.height = 'Height must be greater than zero'
  }
  
  if (!formData.value.weight || formData.value.weight <= 0) {
    errors.value.weight = 'Weight must be greater than zero'
  }
  
  if (!formData.value.superpowerIds || formData.value.superpowerIds.length === 0) {
    errors.value.superpowerIds = 'At least one superpower is required'
  }
  
  return Object.keys(errors.value).length === 0
}

async function handleSubmit() {
  if (!validateForm()) {
    notification.showWarning('Validation Error', 'Please fix the errors in the form', 3000)
    return
  }

  loading.value = true
  
  try {
    // Convert date to ISO string
    if (selectedDate.value) {
      formData.value.birthDate = selectedDate.value.toISOString()
    }

    if (isEditMode.value) {
      await heroStore.updateHero(heroId.value, formData.value)
    } else {
      await heroStore.createHero(formData.value)
    }
    
    // Only navigate if no error occurred
    router.push({ name: 'heroes' })
  } catch (error) {
    // Error toast already shown by interceptor, just stay on page
  } finally {
    loading.value = false
  }
}

function cancel() {
  router.back()
}
</script>

<template>
  <div class="hero-form">
    <div class="header">
      <h2>
        <i :class="isEditMode ? 'pi pi-pencil' : 'pi pi-plus'"></i>
        {{ isEditMode ? 'Edit Hero' : 'New Hero' }}
      </h2>
    </div>

    <Card>
      <template #content>
        <form @submit.prevent="handleSubmit" class="form-grid">
          <div class="form-section">
            <h3>Personal Information</h3>
            
            <div class="form-field">
              <label for="name">Real Name *</label>
              <InputText 
                id="name"
                v-model="formData.name" 
                :class="{ 'p-invalid': errors.name }"
                placeholder="Enter real name"
              />
              <small v-if="errors.name" class="p-error">{{ errors.name }}</small>
            </div>

            <div class="form-field">
              <label for="heroName">Hero Name *</label>
              <InputText 
                id="heroName"
                v-model="formData.heroName" 
                :class="{ 'p-invalid': errors.heroName }"
                placeholder="Enter hero name"
              />
              <small v-if="errors.heroName" class="p-error">{{ errors.heroName }}</small>
            </div>

            <div class="form-field">
              <label for="birthDate">Birth Date *</label>
              <Calendar 
                id="birthDate"
                v-model="selectedDate" 
                :class="{ 'p-invalid': errors.birthDate }"
                dateFormat="mm/dd/yy"
                :maxDate="new Date()"
                showIcon
                placeholder="Select birth date"
              />
              <small v-if="errors.birthDate" class="p-error">{{ errors.birthDate }}</small>
            </div>
          </div>

          <div class="form-section">
            <h3>Physical Attributes</h3>
            
            <div class="form-field">
              <label for="height">Height (meters) *</label>
              <InputNumber 
                id="height"
                v-model="formData.height" 
                :class="{ 'p-invalid': errors.height }"
                mode="decimal"
                :minFractionDigits="2"
                :maxFractionDigits="2"
                :min="0"
                :step="0.01"
                placeholder="Enter height"
              />
              <small v-if="errors.height" class="p-error">{{ errors.height }}</small>
            </div>

            <div class="form-field">
              <label for="weight">Weight (kg) *</label>
              <InputNumber 
                id="weight"
                v-model="formData.weight" 
                :class="{ 'p-invalid': errors.weight }"
                mode="decimal"
                :minFractionDigits="1"
                :maxFractionDigits="1"
                :min="0"
                :step="0.1"
                placeholder="Enter weight"
              />
              <small v-if="errors.weight" class="p-error">{{ errors.weight }}</small>
            </div>
          </div>

          <div class="form-section full-width">
            <h3>Superpowers</h3>
            
            <div class="form-field">
              <label for="superpowers">Superpowers *</label>
              <MultiSelect 
                id="superpowers"
                v-model="formData.superpowerIds" 
                :options="superpowerStore.superpowers"
                optionLabel="superpower"
                optionValue="id"
                :class="{ 'p-invalid': errors.superpowerIds }"
                placeholder="Select superpowers"
                display="chip"
                :loading="superpowerStore.loading"
              />
              <small v-if="errors.superpowerIds" class="p-error">{{ errors.superpowerIds }}</small>
            </div>
          </div>

          <div class="form-actions full-width">
            <Button 
              label="Cancel" 
              icon="pi pi-times" 
              severity="secondary"
              @click="cancel"
              type="button"
              :disabled="loading"
            />
            <Button 
              :label="isEditMode ? 'Update' : 'Create'" 
              icon="pi pi-check" 
              type="submit"
              :loading="loading"
            />
          </div>
        </form>
      </template>
    </Card>
  </div>
</template>

