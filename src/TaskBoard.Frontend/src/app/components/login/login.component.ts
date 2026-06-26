import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { WorkspaceService } from '../../services/workspace.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatCard, MatFormField, MatInput, MatButton, MatLabel],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private fb = inject(FormBuilder);

 private authService = inject(AuthService);
 private workspaceService = inject(WorkspaceService);
 private router = inject(Router);
 
  loginForm = this.fb.nonNullable.group({
    email: ['', { validators: [Validators.required, Validators.email] }],
    password: ['', { validators: [Validators.required] }],
  });
  value = this.loginForm.getRawValue();

  onSubmit() {
      this.authService.login({email: this.value.email, password: this.value.password}).subscribe({
        next: (response) => {
          console.log('Login successful:', response);

          this.workspaceService.getWorkspaces().subscribe({

            next: (workspaces) => {
              console.log('Workspaces loaded:', workspaces);
              this.workspaceService.setWorkspaces(workspaces);
              this.router.navigate(['/dashboard']);
            },
            error: (error) => console.error('Failed to load workspaces:', error)
          });
         },
        error: (error) => {
          console.error('Login failed:', error);
         }
      });
    }
  
}
