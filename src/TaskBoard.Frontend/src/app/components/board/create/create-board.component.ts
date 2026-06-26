import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { BoardService } from '../../../services/board.service';

@Component({
  selector: 'app-create-board',
  imports: [ReactiveFormsModule, MatFormField, MatLabel, MatCard, MatButton, MatInput],
  templateUrl: './create-board.component.html',
  styleUrl: './create-board.component.css'
})
export class CreateBoardComponent {
  private fb = inject(FormBuilder);
  private boardService = inject(BoardService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  workspaceId = this.route.snapshot.paramMap.get('workspaceId')!;
  createBoardForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: ['']
  });

  onSubmit() {
    const value = this.createBoardForm.getRawValue();
    this.boardService.createBoard(this.workspaceId, value).subscribe({
      next: (board: any) => this.router.navigate(['/workspaces', this.workspaceId, 'boards', board.id]),
      error: (error) => console.error('Failed to create board:', error)
    });
  }
}
