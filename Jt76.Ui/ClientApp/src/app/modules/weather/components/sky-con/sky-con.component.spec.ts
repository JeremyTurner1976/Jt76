import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkyConComponent } from './sky-con.component';

describe('SkyConComponent', () => {
  let component: SkyConComponent;
  let fixture: ComponentFixture<SkyConComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkyConComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkyConComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
