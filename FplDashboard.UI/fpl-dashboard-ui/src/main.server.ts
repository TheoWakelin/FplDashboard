import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideAnimations } from '@angular/platform-browser/animations';

const bootstrap = (context: BootstrapContext) =>
    bootstrapApplication(AppComponent, {
        providers: [provideAnimations()]
    }, context);

export default bootstrap;
