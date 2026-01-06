import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseService } from '../../services/course.service';
import { CourseEnrollment, CourseOffer } from '../../models/course.model';

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './courses.html',
  styleUrl: './courses.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CoursesComponent implements OnInit {
  readonly offers = signal<CourseOffer[]>([]);
  readonly enrollments = signal<CourseEnrollment[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  private readonly period = '2024-1';

  constructor(private readonly courseService: CourseService) {}

  ngOnInit(): void {
    this.loadData();
  }

  enroll(offer: CourseOffer): void {
    this.error.set(null);
    this.courseService.enrollCourse(offer.subjectId, offer.period).subscribe({
      next: () => {
        this.loadEnrollments();
      },
      error: (err) => {
        const message = err?.error?.error ?? 'No se pudo inscribir al cursado.';
        this.error.set(message);
      },
    });
  }

  private loadData(): void {
    this.loading.set(true);
    this.courseService.getCourseOffers(this.period).subscribe({
      next: (offers) => {
        this.offers.set(offers);
        this.loadEnrollments();
      },
      error: () => {
        this.error.set('No se pudieron cargar las ofertas.');
        this.loading.set(false);
      },
    });
  }

  private loadEnrollments(): void {
    this.courseService.getMyCourseEnrollments().subscribe({
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
