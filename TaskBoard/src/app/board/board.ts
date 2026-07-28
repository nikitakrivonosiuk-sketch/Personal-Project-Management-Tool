import { Component } from '@angular/core';
import { TaskList } from './list/list-item';
import { List } from "./list/list";

@Component({
  selector: 'app-board',
  imports: [List],
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board {
  boardLists: TaskList[] = [{
    id: '1',
    title: 'To Do',
    cards: [{
      id: '1',
      title: 'Create an app',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-30',
      priority: 'medium',
      status: 'todo',
      },
      {
      id: '2',
      title: 'Card Name',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-29',
      priority: 'low',
      status: 'in progress',
      }
    ],
  },
  
  { id: '2',
    title: 'In Progress',
    cards: [{
      id: '1',
      title: 'Fixing bugs',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-30',
      priority: 'medium',
      status: 'todo',
    }],
  }]
}
