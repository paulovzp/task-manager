import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/auth.service';

@Component({ selector: 'app-login', imports: [ReactiveFormsModule, RouterLink], templateUrl: './login.component.html', styleUrl: './auth.scss' })
export class LoginComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);
  readonly busy = signal(false);
  readonly error = signal('');
  readonly form = this.formBuilder.nonNullable.group({
    email: ['demo@taskmanager.local', [Validators.required, Validators.email]],
    password: ['Demo1234', Validators.required],
  });

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.busy.set(true);
    this.error.set('');
    const { email, password } = this.form.getRawValue();
    this.auth.login(email, password).pipe(finalize(() => this.busy.set(false))).subscribe({
      next: () => void this.router.navigate(['/tasks']),
      error: (error: HttpErrorResponse) => this.error.set(error.error?.detail ?? 'We could not sign you in. Check your details and try again.'),
    });
  }
}
