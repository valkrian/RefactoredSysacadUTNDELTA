//impos from angular

import { Component } from '@angular/core';
//router outlet = shows content of the routes
import { RouterOutlet } from '@angular/router';
//  directives for navigator
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({

  //name for the component (use html)
  selector: 'app-root',

//imports components and directives

imports: [
  RouterOutlet, //shows route content 
  RouterLink,   //shows navigation links
  RouterLinkActive, // shows active route
],

//HTML route for template

templateUrl: './app.html',

// css style routes

styleUrl: './app.css',

})

// component class (logic)
export class App {

  protected readonly title = 'Autogestión Alumnos';
}
