import { useToast } from 'primevue/usetoast'

// Track recent notifications globally to avoid duplicates
const recentNotifications = new Set<string>()

export interface NotificationOptions {
  severity?: 'success' | 'info' | 'warn' | 'error'
  summary: string
  detail: string
  life?: number
}

/**
 * Global notification composable with duplicate prevention
 * 
 * Usage:
 * ```typescript
 * const { showNotification } = useNotification()
 * 
 * showNotification({
 *   severity: 'success',
 *   summary: 'Success',
 *   detail: 'Operation completed successfully'
 * })
 * ```
 */
export function useNotification() {
  const toast = useToast()

  /**
   * Show a notification toast. Automatically prevents duplicate messages
   * from appearing within a short time window.
   */
  function showNotification(options: NotificationOptions) {
    const {
      severity = 'info',
      summary,
      detail,
      life = 5000
    } = options

    const key = `${severity}:${summary}:${detail}`

    // Check if this exact notification was recently shown
    if (!recentNotifications.has(key)) {
      recentNotifications.add(key)

      toast.add({
        severity,
        summary,
        detail,
        life
      })

      // Remove from set after the toast disappears
      setTimeout(() => {
        recentNotifications.delete(key)
      }, life + 500)
    }
  }

  /**
   * Show a success notification
   */
  function showSuccess(summary: string, detail: string, life?: number) {
    showNotification({ severity: 'success', summary, detail, life })
  }

  /**
   * Show an info notification
   */
  function showInfo(summary: string, detail: string, life?: number) {
    showNotification({ severity: 'info', summary, detail, life })
  }

  /**
   * Show a warning notification
   */
  function showWarning(summary: string, detail: string, life?: number) {
    showNotification({ severity: 'warn', summary, detail, life })
  }

  /**
   * Show an error notification
   */
  function showError(summary: string, detail: string, life?: number) {
    showNotification({ severity: 'error', summary, detail, life })
  }

  return {
    showNotification,
    showSuccess,
    showInfo,
    showWarning,
    showError
  }
}

/**
 * Internal API for the axios interceptor to use without needing a Vue component context
 */
let globalToastInstance: any = null

export function setGlobalToastInstance(toast: any) {
  globalToastInstance = toast
}

export function showGlobalNotification(options: NotificationOptions) {
  if (!globalToastInstance) {
    //console.warn('Global toast instance not set')
    return
  }

  const {
    severity = 'info',
    summary,
    detail,
    life = 5000
  } = options

  const key = `${severity}:${summary}:${detail}`

  if (!recentNotifications.has(key)) {
    recentNotifications.add(key)

    globalToastInstance.add({
      severity,
      summary,
      detail,
      life
    })

    setTimeout(() => {
      recentNotifications.delete(key)
    }, life + 500)
  }
}
