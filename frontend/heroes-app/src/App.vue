<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterView } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { setGlobalToastInstance } from './composables/useNotification'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import './assets/styles/App.css'

const toast = useToast()

onMounted(() => {
  setGlobalToastInstance(toast)
})
</script>

<template>
  <div class="app-container">
    <header class="app-header">
      <div class="header-content">
        <h1>
          <i class="pi pi-bolt"></i>
          Heroes Management
        </h1>
        <nav>
          <RouterLink to="/heroes" class="nav-link">
            <i class="pi pi-users"></i>
            Heroes
          </RouterLink>
        </nav>
      </div>
    </header>
    
    <main class="app-main">
      <RouterView />
    </main>
    
    <Toast />
    <ConfirmDialog />
  </div>
</template>
