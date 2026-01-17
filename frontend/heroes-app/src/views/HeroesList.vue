<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useHeroStore } from '../stores/heroStore'
import { useConfirm } from 'primevue/useconfirm'
import { formatDateShort } from '../utils/formatters'
import type { Hero } from '../types'
import '../assets/styles/views/HeroesList.css'

import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'

const router = useRouter()
const heroStore = useHeroStore()
const confirm = useConfirm()

const searchQuery = ref('')

const filteredHeroes = computed(() => {
  if (!searchQuery.value) return heroStore.heroes
  
  const query = searchQuery.value.toLowerCase()
  return heroStore.heroes.filter(hero => 
    hero.name.toLowerCase().includes(query) ||
    hero.heroName.toLowerCase().includes(query)
  )
})

onMounted(async () => {
  await heroStore.fetchHeroes()
})

function viewHero(hero: Hero) {
  router.push({ name: 'hero-detail', params: { id: hero.id } })
}

function editHero(hero: Hero) {
  router.push({ name: 'hero-edit', params: { id: hero.id } })
}

function confirmDelete(hero: Hero) {
  confirm.require({
    message: `Are you sure you want to delete ${hero.heroName}?`,
    header: 'Confirm Delete',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    accept: async () => {
      try {
        await heroStore.deleteHero(hero.id)
      } catch (error) {
        // Error toast already shown by interceptor
      }
    }
  })
}

function createHero() {
  router.push({ name: 'hero-create' })
}
</script>

<template>
  <div class="heroes-list">
    <div class="header">
      <h2>
        <i class="pi pi-users"></i>
        Heroes List
      </h2>
      <Button 
        label="New Hero" 
        icon="pi pi-plus" 
        @click="createHero"
        severity="success"
      />
    </div>

    <div class="search-bar">
      <IconField iconPosition="left">
        <InputIcon class="pi pi-search" />
        <InputText 
          v-model="searchQuery" 
          placeholder="Search heroes..." 
          class="w-full"
        />
      </IconField>
    </div>

    <DataTable 
      :value="filteredHeroes" 
      :loading="heroStore.loading"
      stripedRows
      tableStyle="min-width: 50rem"
    >
      <Column field="heroName" header="Hero Name" sortable>
        <template #body="{ data }">
          <strong>{{ data.heroName }}</strong>
        </template>
      </Column>
      <Column field="name" header="Real Name" sortable></Column>
      <Column field="birthDate" header="Birth Date" sortable>
        <template #body="{ data }">
          {{ formatDateShort(data.birthDate) }}
        </template>
      </Column>
      <Column field="height" header="Height (m)" sortable>
        <template #body="{ data }">
          {{ data.height.toFixed(2) }}
        </template>
      </Column>
      <Column field="weight" header="Weight (kg)" sortable>
        <template #body="{ data }">
          {{ data.weight.toFixed(1) }}
        </template>
      </Column>
      <Column header="Actions" :exportable="false">
        <template #body="{ data }">
          <div class="actions">
            <Button 
              icon="pi pi-eye" 
              rounded
              text
              severity="info"
              @click="viewHero(data)"
              v-tooltip.top="'View Details'"
            />
            <Button 
              icon="pi pi-pencil" 
              rounded
              text
              severity="warning"
              @click="editHero(data)"
              v-tooltip.top="'Edit'"
            />
            <Button 
              icon="pi pi-trash" 
              rounded
              text
              severity="danger"
              @click="confirmDelete(data)"
              v-tooltip.top="'Delete'"
            />
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

