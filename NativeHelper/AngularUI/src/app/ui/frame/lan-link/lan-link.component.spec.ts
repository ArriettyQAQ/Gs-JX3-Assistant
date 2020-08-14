import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LanLinkComponent } from './lan-link.component';

describe('LanLinkComponent', () => {
  let component: LanLinkComponent;
  let fixture: ComponentFixture<LanLinkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LanLinkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LanLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
