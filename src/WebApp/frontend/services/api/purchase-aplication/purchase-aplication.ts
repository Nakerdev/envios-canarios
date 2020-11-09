import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { PurchaseAplicationApi, PurchaseAplicationResponse } from './models/purchase-aplication.models'

export function sendPurchaseAplication ({ url, axios, body }: { axios: NuxtAxiosInstance, url: string, body: PurchaseAplicationApi }): Promise<PurchaseAplicationResponse> {
  return axios.$post(url, body, {
    headers: {
      'Access-Control-Allow-Origin': '*'
    }
  })
}
