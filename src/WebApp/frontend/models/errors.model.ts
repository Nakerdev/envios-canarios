export interface BadRequestResponse {
  validationErrors?: ValidationError[]
  errorCode?: OperationError
}

export interface ValidationError {
  field: string
  errorCode: string
}

export type OperationError = string
