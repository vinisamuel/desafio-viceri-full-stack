<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useHeroStore } from '../stores/heroStore'
import { useConfirm } from 'primevue/useconfirm'
import { formatDate, formatDateTime } from '../utils/formatters'
import '../assets/styles/views/HeroDetail.css'

import Card from 'primevue/card'
import Button from 'primevue/button'
import Skeleton from 'primevue/skeleton'

const router = useRouter()
const route = useRoute()
const heroStore = useHeroStore()
const confirm = useConfirm()

const heroId = Number(route.params.id)

onMounted(async () => {
  await heroStore.fetchHeroById(heroId)
  if (!heroStore.currentHero) {
    router.push({ name: 'heroes' })
  }
})

function editHero() {
  router.push({ name: 'hero-edit', params: { id: heroId } })
}

function confirmDelete() {
  confirm.require({
    message: `Are you sure you want to delete ${heroStore.currentHero?.heroName}?`,
    header: 'Confirm Delete',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    accept: async () => {
      try {
        await heroStore.deleteHero(heroId)
        router.push({ name: 'heroes' })
      } catch (error) {
        // Error toast already shown by interceptor, stay on page
      }
    }
  })
}

function goBack() {
  router.push({ name: 'heroes' })
}
</script>

<template>
  <div class="hero-detail">
    <div class="header">
      <div class="title-section">
        <Button 
          icon="pi pi-arrow-left" 
          text
          rounded
          @click="goBack"
          v-tooltip.right="'Back to list'"
        />
        <h2 v-if="heroStore.currentHero">
          <i class="pi pi-user"></i>
          {{ heroStore.currentHero.heroName }}
        </h2>
        <Skeleton v-else width="300px" height="2rem" />
      </div>
      <div class="actions" v-if="heroStore.currentHero">
        <Button 
          label="Edit" 
          icon="pi pi-pencil" 
          severity="warning"
          @click="editHero"
        />
        <Button 
          label="Delete" 
          icon="pi pi-trash" 
          severity="danger"
          @click="confirmDelete"
        />
      </div>
    </div>

    <div v-if="heroStore.loading" class="loading-state">
      <Card>
        <template #content>
          <Skeleton height="2rem" class="mb-3" />
          <Skeleton height="1.5rem" class="mb-2" />
          <Skeleton height="1.5rem" class="mb-2" />
          <Skeleton height="1.5rem" />
        </template>
      </Card>
    </div>

    <div v-else-if="heroStore.currentHero" class="content-grid">
      <Card class="info-card">
        <template #title>
          <div class="card-title">
            <i class="pi pi-id-card"></i>
            Personal Information
          </div>
        </template>
        <template #content>
          <div class="info-grid">
            <div class="info-item">
              <label>Real Name</label>
              <p>{{ heroStore.currentHero.name }}</p>
            </div>
            <div class="info-item">
              <label>Hero Name</label>
              <p class="hero-name">{{ heroStore.currentHero.heroName }}</p>
            </div>
            <div class="info-item">
              <label>Birth Date</label>
              <p>{{ formatDate(heroStore.currentHero.birthDate) }}</p>
            </div>
            <div class="info-item">
              <label>ID</label>
              <p>#{{ heroStore.currentHero.id }}</p>
            </div>
          </div>
        </template>
      </Card>

      <Card class="info-card">
        <template #title>
          <div class="card-title">
            <i class="pi pi-chart-line"></i>
            Physical Attributes
          </div>
        </template>
        <template #content>
          <div class="info-grid">
            <div class="info-item">
              <label>Height</label>
              <p>{{ heroStore.currentHero.height.toFixed(2) }} meters</p>
            </div>
            <div class="info-item">
              <label>Weight</label>
              <p>{{ heroStore.currentHero.weight.toFixed(1) }} kg</p>
            </div>
          </div>
        </template>
      </Card>

      <Card class="info-card full-width">
        <template #title>
          <div class="card-title">
            <i class="pi pi-bolt"></i>
            Superpowers
          </div>
        </template>
        <template #content>
          <div class="superpowers-grid">
            <div 
              v-for="superpower in heroStore.currentHero.superpowers" 
              :key="superpower.id"
              class="superpower-card"
            >
              <div class="superpower-header">
                <i class="pi pi-star-fill"></i>
                <strong>{{ superpower.superpower }}</strong>
              </div>
              <p>{{ superpower.description }}</p>
            </div>
          </div>
        </template>
      </Card>

      <Card class="info-card full-width metadata">
        <template #title>
          <div class="card-title">
            <i class="pi pi-clock"></i>
            Metadata
          </div>
        </template>
        <template #content>
          <div class="metadata-grid">
            <div class="info-item">
              <label>Created At</label>
              <p>{{ formatDateTime(heroStore.currentHero.createdAt) }}</p>
            </div>
            <div class="info-item">
              <label>Last Updated</label>
              <p>{{ formatDateTime(heroStore.currentHero.updatedAt) }}</p>
            </div>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>

