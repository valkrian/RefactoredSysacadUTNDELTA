# WebAngular

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 21.0.3.

## App structure and layout

- The root component uses RouterOutlet and RouterLink/RouterLinkActive with its template in `src/app/app.html` and styles in `src/app/app.css`.
- The layout is a fixed left sidebar with navigation links and a main area with a top bar and a "Usuario" button; routed views render via `<router-outlet>`.
- The sidebar includes links for Dashboard, Plan, Estado, Examenes, and Cursado (only the Dashboard route is wired today).

## Routes

- `/` redirects to `/dashboard`.
- The Dashboard view is lazy-loaded from `src/app/components/dashboard`.

## Dashboard

- The dashboard template shows three placeholder cards: Plan de Estudios, Estado Academico, and Examenes.

## Styles

- `src/styles.css` sets global box sizing, base font stack, background color, and `main` margin.
- `src/app/app.css` styles the fixed sidebar, active/hover nav states, and the "Usuario" button theme.

## SSR server

- `src/server.ts` serves static assets from the `browser` build output with long cache headers.
- Non-static requests are rendered by Angular SSR.
- The server listens on `PORT` (default 4000) when run directly.
- `reqHandler` is exported for Angular CLI and hosting integrations.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
