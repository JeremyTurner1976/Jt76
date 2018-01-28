import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Jt76.Ui";
  theme: string;
  themeClass: string;
  isAuthenticated = false;
  year = new Date().getFullYear();

  ngOnInit(): void {
    this.setTheme("dark");
  }

  setTheme(theme) {
    this.theme = theme;
    this.themeClass = theme + "-theme";
  }

  colorTheme() {
    var standardClasses = "content mat-card";
    switch (this.theme) {
      case "dark":
        return `dark-colors ${standardClasses}`;
      case "default":
        return `default-colors ${standardClasses}`;
      case "light":
        return `light-colors ${standardClasses}`;
    }
  }

  onLoginClick() {
    this.isAuthenticated = true;
  }

  onLogoutClick() {
    this.isAuthenticated = false;
  }
}
