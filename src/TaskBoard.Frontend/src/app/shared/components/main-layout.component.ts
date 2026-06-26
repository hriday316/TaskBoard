import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { WorkspaceService } from '../../services/workspace.service';
import { MatButtonModule } from '@angular/material/button';
 

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatButtonModule ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
})
export class MainLayoutComponent {
  private workspaceService = inject(WorkspaceService);
  private router = inject(Router);

  workspaces = this.workspaceService.workspaces;
  selectedWorkspace = this.workspaceService.selectedWorkspace;

  ngOnInit() {
    if (this.workspaces().length > 0) return;

    this.workspaceService.getWorkspaces().subscribe({

      next: (res) => {console.log('Workspaces loaded successfully:', res);},
      error: (error) => console.error('Failed to load workspaces:', error)
    });
  }

  changeWorkspace(event: Event) {
    const workspaceId = (event.target as HTMLSelectElement).value;
    const workspace = this.workspaces().find(x => x.id === workspaceId);

    if (workspace) {
      this.workspaceService.selectWorkspace(workspace);
      this.router.navigate(['/dashboard']);
    }
  }

  isBoardPage() {
    return this.router.url.includes('/boards/') && !this.router.url.endsWith('/create');
  }
}
