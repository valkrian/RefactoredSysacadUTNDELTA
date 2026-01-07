import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-help',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './help.html',
    styleUrl: './help.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    })
    export class HelpComponent {
    submitted = false;
    topic = '';
    message = '';

    submit(): void {
        this.submitted = true;
    }
}
