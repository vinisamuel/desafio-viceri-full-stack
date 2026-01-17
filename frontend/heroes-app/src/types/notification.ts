export enum NotificationType {
  Information = 1,
  Warning = 2,
  InvalidParameter = 3,
  Error = 4,
  UntreatedException = 99
}

export interface Notification {
  date: string
  typeId: NotificationType
  type: string
  message: string
}

/**
 * Map NotificationType to PrimeVue Toast severity
 */
export function mapNotificationToSeverity(typeId: NotificationType): 'success' | 'info' | 'warn' | 'error' {
  switch (typeId) {
    case NotificationType.Information:
      return 'success'
    case NotificationType.Warning:
      return 'warn'
    case NotificationType.InvalidParameter:
    case NotificationType.Error:
    case NotificationType.UntreatedException:
      return 'error'
    default:
      return 'info'
  }
}
