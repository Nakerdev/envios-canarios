import { ProductVm } from './product.model'

export const emptyProductVm = (): ProductVm => ({
  id: undefined,
  link: '',
  units: '',
  additionalInformation: undefined,
  promotionCode: undefined
})
