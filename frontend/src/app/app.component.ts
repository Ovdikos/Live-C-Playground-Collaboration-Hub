import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet], // Обов'язково тут!
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent { }
