import { BadRequestResponse } from '../../../models/errors.model'

export interface PurchaseAplicationApi {
  products: ProductApi[]
  client: ClientApi
  additionalInformation?: string /** Must contains a maximum of 1000 characters */
}

export interface ProductApi {
  link: string /** Must be a valid URL and contains a maximum of 1000 characters */
  units: string /** Must be a numeric value greather than 0 */
  additionalInformation?: string /**  Must contains a maximum of 1000 characters */
  promotionCode?: string /** Must contains a maximum of 50 characters */
}

export interface ClientApi {
  name: string /** Must contains a maximum of 255 characters */
  phoneNumber? : string /** Must be a numeric value and contains a maximum of 15 characters */
  email: string /** Must be a valid email format and contains a maximum of 255 characters */
}

export type PurchaseAplicationResponse = void | BadRequestResponse;
