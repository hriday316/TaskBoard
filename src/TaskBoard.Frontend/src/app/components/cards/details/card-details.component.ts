import { Component, inject, signal } from '@angular/core';
import { CardService } from '../../../services/card.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommentService } from '../../../services/comment.service';
 

@Component({
  selector: 'app-card-details',
  imports: [ReactiveFormsModule],
  templateUrl: './card-details.component.html',
  styleUrl: './card-details.component.css',
})
export class CardDetailsComponent {
  private route = inject(ActivatedRoute);
  private cardService = inject(CardService);
  private fb = inject(FormBuilder);
  private commentService = inject(CommentService);

  workspaceId = this.route.snapshot.paramMap.get('workspaceId')!;
  boardId = this.route.snapshot.paramMap.get('boardId')!;
  cardId = this.route.snapshot.paramMap.get('cardId')!;

  card = signal<any>(null);
  creatingComment = signal<boolean>(false);
  comments = signal<any>(null);

  form = this.fb.nonNullable.group({
     message: ['', { validators: [Validators.required, Validators.minLength(2)] }],
  });




  ngOnInit() {
    this.loadCardDetails();
    this.loadComments();
  }

  loadCardDetails() {
    this.cardService.getCard(this.workspaceId, this.boardId, this.cardId).subscribe({
      next: (card: any) => {
        console.log('Card details loaded successfully:', card);
        this.card.set(card);
       },
      error: (error) => console.error('Failed to load card details:', error),
    });
  }

  goBack() {
    window.history.back();
  }
 

  onSubmit() {
    const message = this.form.value.message;
    if(this.form.valid && message) {
      this.commentService.createComment(this.cardId,this.workspaceId, message).subscribe({
        next: (comment: any) => {
          console.log('Comment created successfully:', comment);
          this.loadComments();
          this.form.reset();
        },
        error: (error) => console.error('Failed to create comment:', error),
      });
    } 
  }

  loadComments() {
    this.commentService.getComments(this.cardId).subscribe({
      next: (res) => {
        console.log('Comments loaded successfully:', res);
        this.comments.set(res);
      },
      error: (error) => console.error('Failed to load comments:', error),
    });
  }


}