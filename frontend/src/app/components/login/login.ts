import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  readonly error = signal<string | null>(null);
  readonly loading = signal(false);

  email = '';
  password = '';

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  submit(): void {
    this.error.set(null);
    this.loading.set(true);

    this.authService.login(this.email.trim(), this.password).subscribe({
      next: () => {
        this.authService.loadMe().subscribe({
          next: () => {
            this.loading.set(false);
            this.router.navigateByUrl('/dashboard');
          },
          error: () => {
            this.loading.set(false);
            this.router.navigateByUrl('/dashboard');
          },
        });
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Credenciales invalidas.');
      },
    });
  }
}
