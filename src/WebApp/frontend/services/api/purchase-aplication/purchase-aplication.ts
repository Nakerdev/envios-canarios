import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { PurchaseAplication, PurchaseAplicationResponse } from './models/purchase-aplication.models'

export function sendPurchaseAplication ({ url, axios, body }: { axios: NuxtAxiosInstance, url: string, body: PurchaseAplication }): Promise<PurchaseAplicationResponse> {
  return axios.$post(url, body, {
    headers: {
      'Access-Control-Allow-Origin': '*'
    }
  })
}
