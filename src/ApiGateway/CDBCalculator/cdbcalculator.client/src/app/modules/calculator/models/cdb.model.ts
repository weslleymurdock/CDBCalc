export interface CdbSimulationRequest {
  months: number;
  initialValue: number;
}

export interface CdbSimulationResponse {
  gross: number;
  net: number;
  success: boolean;
  message?: string;
  statusCode: number;
}
