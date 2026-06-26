import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MainLayoutComponent } from './shared/components/main-layout.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { CreateWorkspaceComponent } from './components/workspace/create/create-workspace.component';
import { BoardComponent } from './components/board/board.component';
import { CreateBoardComponent } from './components/board/create/create-board.component';
import { RegisterComponent } from './components/register/register.component';
import { CardDetailsComponent } from './components/cards/details/card-details.component';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'login',
        
    },
    {
        path: 'login',
        component : LoginComponent

    },
    {
        path: 'register',
        component : RegisterComponent
    },
    {
        path:'',
        component: MainLayoutComponent,
        children: [
            {   
            path: 'dashboard',
            component:  DashboardComponent
            },
            {
                path: 'workspaces/create',
                component: CreateWorkspaceComponent
            },
            {
                path: 'workspaces/:workspaceId/boards/create',
                component: CreateBoardComponent
            },
            {
                path: 'workspaces/:workspaceId/boards/:boardId',
                component: BoardComponent
            },
            {
                path: 'workspaces/:workspaceId/boards/:boardId/cards/:cardId',
                component: CardDetailsComponent
            }
        ]
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];
