import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogClose, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormField, MatLabel } from "@angular/material/form-field";
import { MatInput } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { WorkspaceService } from '../../../services/workspace.service';
import { MatIcon } from '@angular/material/icon';
import { MatSelect } from '@angular/material/select';
import { CardService } from '../../../services/card.service';
import { BoardService } from '../../../services/board.service';

export interface CreateCardModalComponentData {
  boardId: string;
  columnId: string;
  leadId: string;
  workspaceId: string;
}

@Component({
  selector: 'app-create-card-modal',
  standalone: true,
  imports: [
    MatButton
     ,
    MatDialogTitle,
    MatFormField,
    MatIcon,
    MatInput,
    MatLabel,
    MatOptionModule,
    MatSelect,
    ReactiveFormsModule,
  ],
  templateUrl: './create-card-modal.component.html',
  styleUrl: './create-card-modal.component.css',
})
export class CreateCardModalComponent {
  private fb = inject(FormBuilder);
  private  dialogRef = inject(MatDialogRef<CreateCardModalComponent>);
  private workspaceService = inject(WorkspaceService);
  private boardService = inject(BoardService);
  private cardService = inject(CardService);
  private data = inject<CreateCardModalComponentData>(MAT_DIALOG_DATA);
  board = this.boardService.board;
  selectedFiles: File[] = [];
  members: any[] = [];

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    description: [''],
    assignMemberId: [''],
  });

  onFileSelected(event:  Event) {
    const input = event.target as HTMLInputElement;
    if(!input.files || input.files.length ===0) {
      return;
    }
          const newFiles = Array.from(input.files); 

    this.selectedFiles.push(...newFiles);
    input.value = ''; 

  }
  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }
      const formValue = this.form.getRawValue();
      const formData = new FormData();
      formData.append('Title', formValue.title);
      formData.append('Description', formValue?.description ?? '');
      formData.append('AssignMemberId', formValue?.assignMemberId ?? '');
      
      formData.append('BoardColumnId', this.data.columnId);
      formData.append('LaneId', this.data.leadId);
       

      for (const file of this.selectedFiles) {
        formData.append('Attachments', file, file.name);
      }

      this.cardService.createCard(
        this.data.workspaceId,
        this.data.boardId,
        this.data.columnId,
        this.data.leadId,
        formData
      ).subscribe({
        next: (response) => {
          console.log('Card created successfully:', response);
          this.dialogRef.close(true);
             

        },
        error: (error) => {
          console.error('Failed to create card:', error);
          this.dialogRef.close();
        }
      });

       
    }
  

  close() {
    this.dialogRef.close();
  }
  ngOnInit() {
    const workspaceId = localStorage.getItem('workspaceId');
    if (workspaceId) {
      this.getWorkspaceMembers(workspaceId);
    }
  }
  getWorkspaceMembers(workspaceId: string) {
    this.workspaceService.getWorkspaceMembers().subscribe({
      next: (members: any[]) => {
        this.members = members;
        console.log('Workspace members loaded successfully:', members);
      },
      error: (error) => console.error('Failed to load workspace members:', error)
    });

  }




}
