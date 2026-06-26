import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { WorkspaceService } from '../../services/workspace.service';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { BoardService } from '../../services/board.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatButton, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent {
  private workspaceService = inject(WorkspaceService);
  private boardService = inject(BoardService);
  private router = inject(Router);

  boards = signal<any[]>([]);
  selectedWorkspace = this.workspaceService.selectedWorkspace;

  constructor() {
    effect(() => {
      const workspace = this.selectedWorkspace();
      if (workspace) {
        this.boards.set([]);
        this.loadBoards(workspace.id);
      } else {
        this.boards.set([]);
      }
    });
  }

  loadBoards(workspaceId: string) {
    this.boardService.getBoards(workspaceId).subscribe({
      next: (response: any) => this.boards.set(response),
      error: (error) => console.error('Failed to load boards:', error)
    });
  }
  goToBoardDetails(boardId: string) {
    this.router.navigate([`/workspaces/${this.selectedWorkspace()?.id}/boards/${boardId}`]);
  }
}
