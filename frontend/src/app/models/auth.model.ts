export interface LoginResponse {
  token: string;
  studentId: number;
  fullName: string;
  email: string;
}

export interface MeResponse {
  studentId: number;
  fullName: string;
  email: string;
  planId: number;
}
