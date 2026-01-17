import { format } from 'date-fns'

/**
 * Format date to full format: "MMMM dd, yyyy"
 * Example: "January 17, 2026"
 */
export function formatDate(dateString: string): string {
  return format(new Date(dateString), 'MMMM dd, yyyy')
}

/**
 * Format date to short format: "MM/dd/yyyy"
 * Example: "01/17/2026"
 */
export function formatDateShort(dateString: string): string {
  return format(new Date(dateString), 'MM/dd/yyyy')
}

/**
 * Format date and time: "MMM dd, yyyy HH:mm"
 * Example: "Jan 17, 2026 14:30"
 */
export function formatDateTime(dateString: string): string {
  return format(new Date(dateString), 'MMM dd, yyyy HH:mm')
}

/**
 * Format number to fixed decimal places
 */
export function formatNumber(value: number, decimals: number = 2): string {
  return value.toFixed(decimals)
}
