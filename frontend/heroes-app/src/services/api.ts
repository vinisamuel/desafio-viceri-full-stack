import axios from 'axios'
import type { Notification } from '../types/notification'
import { mapNotificationToSeverity } from '../types/notification'
import { showGlobalNotification } from '../composables/useNotification'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Response interceptor to handle notifications
api.interceptors.response.use(
  (response) => {
    // Check if response has notifications
    if (response.data && response.data.notifications && Array.isArray(response.data.notifications)) {
      const notifications = response.data.notifications as Notification[]
      
      // Display toast for each notification
      if (notifications.length > 0) {
        notifications.forEach((notification) => {
          showGlobalNotification({
            severity: mapNotificationToSeverity(notification.typeId),
            summary: notification.type,
            detail: notification.message,
            life: 5000
          })
        })
      }
    }
    
    return response
  },
  (error) => {
    if (error.response) {
      // Try to display error notifications if present
      if (error.response.data && error.response.data.notifications) {
        const notifications = error.response.data.notifications as Notification[]
        
        if (notifications.length > 0) {
          notifications.forEach((notification) => {
            showGlobalNotification({
              severity: mapNotificationToSeverity(notification.typeId),
              summary: notification.type,
              detail: notification.message,
              life: 5000
            })
          })
        } else if (error.response.data.message) {
          // Fallback error message
          showGlobalNotification({
            severity: 'error',
            summary: 'Error',
            detail: error.response.data.message || 'An error occurred',
            life: 5000
          })
        }
      }
    }
    return Promise.reject(error)
  }
)

export default api
