import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { PurchaseAplication, PurchaseAplicationResponse } from '../api/purchase-aplication/models/purchase-aplication.models'
import * as API from '../api/purchase-aplication/purchase-aplication'

export async function sendPurchaseAplication ({ axios, body }: { axios: NuxtAxiosInstance, body: PurchaseAplication }): Promise<PurchaseAplicationResponse> {
  const url = '/purchase-aplication'
  console.log('REQUEST', { axios, url, body })
  const response = await API.sendPurchaseAplication({ axios, url, body })
  console.log('RESPONSE', response)
  return response
}
