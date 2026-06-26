import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatCard } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { WorkspaceService } from '../../../services/workspace.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-workspace',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormField, MatLabel, MatCard, MatButton, MatInput],
  templateUrl: './create-workspace.component.html',
  styleUrl: './create-workspace.component.css',
})
export class CreateWorkspaceComponent {
  private fb = inject(FormBuilder);
  private workspaceService = inject(WorkspaceService);
  route = inject(Router);

  createWorkspaceForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: [''],
  });

  onSubmit() {
    const value = this.createWorkspaceForm.getRawValue();

    this.workspaceService
      .createWorkspace({ name: value.name, description: value.description })
      .subscribe({
        next: (response) => {
          console.log('Workspace created successfully:', response);
          this.workspaceService.addWorkspace(response);
          this.route.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Failed to create workspace:', error);
        },
      });
  }
}
