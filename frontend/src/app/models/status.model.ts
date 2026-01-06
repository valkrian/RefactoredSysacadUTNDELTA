export interface StudentStatus {
  studentId: number;
  approvedCount: number;
  regularCount: number;
  pendingCount: number;
  average: number | null;
}
