import { defineStore } from 'pinia'

export const useUserStore = defineStore({
  id: 'token',
  state: () => ({
    token: null
  }),
  
})
