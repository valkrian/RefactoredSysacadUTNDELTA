import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProfileService, Profile } from '../../services/profile.service';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './profile.html',
    styleUrl: './profile.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    })
    export class ProfileComponent implements OnInit {
    readonly profile = signal<Profile | null>(null);
    readonly message = signal<string | null>(null);
    readonly error = signal<string | null>(null);

    fullName = '';
    email = '';
    currentPassword = '';
    newPassword = '';

    constructor(private readonly profileService: ProfileService) {}

    ngOnInit(): void {
        this.profileService.getProfile().subscribe((data) => {
        this.profile.set(data);
        this.fullName = data.fullName;
        this.email = data.email;
        });
    }

    saveProfile(): void {
        this.message.set(null);
        this.error.set(null);

        this.profileService.updateProfile(this.fullName, this.email).subscribe({
        next: () => {
            this.message.set('Datos actualizados correctamente.');
        },
        error: () => {
            this.error.set('No se pudieron guardar los cambios.');
        },
        });
    }

    updatePassword(): void {
        this.message.set(null);
        this.error.set(null);

        this.profileService.changePassword(this.currentPassword, this.newPassword).subscribe({
        next: () => {
            this.message.set('Password actualizada.');
            this.currentPassword = '';
            this.newPassword = '';
        },
        error: (err) => {
            const apiError = err?.error?.error;
            this.error.set(apiError ?? 'No se pudo actualizar la contraseña.');
        },
        });
    }
}
