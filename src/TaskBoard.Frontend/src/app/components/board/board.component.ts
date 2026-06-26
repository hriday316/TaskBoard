import { Component, inject, input, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CdkDrag,
  CdkDragDrop,
  CdkDropList,
  CdkDropListGroup,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { BoardService } from '../../services/board.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateCardModalComponent } from '../cards/create/create-card-modal.component';
import { T } from '@angular/cdk/keycodes';
 
@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CdkDropList, CdkDrag, CdkDropListGroup],
  templateUrl: './board.component.html',
  styleUrl: './board.component.css',
})
export class BoardComponent {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private boardService = inject(BoardService);
  private dialog = inject(MatDialog);

 




  workspaceId = this.route.snapshot.paramMap.get('workspaceId')!;
  boardId = this.route.snapshot.paramMap.get('boardId')!;
  board = this.boardService.board;
  cells: Record<string, any> = {};

  ngOnInit() {
    this.loadBoard();
  }

  loadBoard() {
    this.boardService.getBoard(this.workspaceId, this.boardId).subscribe({
      next: (board: any) => {
        console.log('Board loaded successfully:', board);
        this.cells = {};
        board.cells.forEach((cell: any) => {
          this.cells[this.cellKey(cell.columnId, cell.laneId)] = cell;
        });
        console.log('Cells:', this.cells);
        this.board.set(board);
      },
      error: (error) => console.error('Failed to load board:', error),
    });
  }

  cellKey(columnId: string, laneId: string) {
    return columnId + '-' + laneId;
  }

  getCell(columnId: string, laneId: string) {
    return this.cells[this.cellKey(columnId, laneId)];
  }
  totalColSpan() {
    return this.board().columns.reduce(
      (total: number, column: any) => total + column.columnSpan,
      0,
    );
  }

  drop(event: CdkDragDrop<any>) {
    console.log('Drop event:', event);
    const oldCell = event.previousContainer.data;
    const newCell = event.container.data;

    if (event.previousContainer === event.container) {
      moveItemInArray(newCell.cards, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(oldCell.cards, newCell.cards, event.previousIndex, event.currentIndex);
    }

    const card = newCell.cards[event.currentIndex];
    this.boardService
      .moveCard(this.workspaceId, this.boardId, card.id, {
        targetBoardColumnId: newCell.columnId,
        targetLaneId: newCell.laneId,
        targetOrder: event.currentIndex + 1,
      })
      .subscribe({
        next: (res) => console.log('Card moved successfully', res),
        error: (error) => {
           console.error('Failed to move card:', error);
        },
      });
  }

  openCreateCardModal(columnId: string, laneId: string) {
    const dialogRef = this.dialog.open(CreateCardModalComponent, {
      data: {
        boardId: this.boardId,
        columnId: columnId,
        leadId: laneId,
        workspaceId: this.workspaceId,
      },
    });
    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (created) {
        this.loadBoard();
      }
    });
  }

  goToCardDetails(cardId: string) {
    console.log(`Navigating to card details for card ID: ${cardId}`);
     this.router.navigate([`/workspaces/${this.workspaceId}/boards/${this.boardId}/cards/${cardId}`]);
  }

}
