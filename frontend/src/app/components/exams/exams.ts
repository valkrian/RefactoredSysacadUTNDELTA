import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExamService } from '../../services/exam.service';
import { ExamCall, ExamEnrollment } from '../../models/exam.model';

@Component({
  selector: 'app-exams',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './exams.html',
  styleUrl: './exams.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExamsComponent implements OnInit {
  readonly calls = signal<ExamCall[]>([]);
  readonly enrollments = signal<ExamEnrollment[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  constructor(private readonly examService: ExamService) {}

  ngOnInit(): void {
    this.loadData();
  }

  enroll(call: ExamCall): void {
    this.error.set(null);
    this.examService.enrollExam(call.examCallId).subscribe({
      next: () => {
        this.loadEnrollments();
      },
      error: (err) => {
        const message = err?.error?.error ?? 'No se pudo inscribir a la mesa.';
        this.error.set(message);
      },
    });
  }

  private loadData(): void {
    this.loading.set(true);
    this.examService.getExamCalls().subscribe({
      next: (calls) => {
        this.calls.set(calls);
        this.loadEnrollments();
      },
      error: () => {
        this.error.set('No se pudieron cargar las mesas.');
        this.loading.set(false);
      },
    });
  }

  private loadEnrollments(): void {
    this.examService.getMyExamEnrollments().subscribe({
      next: (enrollments) => {
        this.enrollments.set(enrollments);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se pudieron cargar las inscripciones.');
        this.loading.set(false);
      },
    });
  }
}
