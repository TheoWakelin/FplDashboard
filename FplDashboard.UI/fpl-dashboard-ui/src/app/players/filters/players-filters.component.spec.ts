import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { PlayersFiltersComponent } from './players-filters.component';
import { ApiDataService } from '../../core/services/api-data.service';
import { TeamListDto } from '../../teams/team-list.model';

describe('PlayersFiltersComponent', () => {
  let component: PlayersFiltersComponent;
  let fixture: ComponentFixture<PlayersFiltersComponent>;
  let mockApiService: jasmine.SpyObj<ApiDataService>;

  const mockTeams: TeamListDto[] = [
    { id: 1, name: 'Arsenal' },
    { id: 2, name: 'Chelsea' }
  ];

  beforeEach(async () => {
    mockApiService = jasmine.createSpyObj('ApiDataService', ['getTeamsList']);
    mockApiService.getTeamsList.and.returnValue(of(mockTeams));

    await TestBed.configureTestingModule({
      imports: [PlayersFiltersComponent],
      providers: [{ provide: ApiDataService, useValue: mockApiService }]
    }).compileComponents();

    fixture = TestBed.createComponent(PlayersFiltersComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load teams list on init', () => {
    fixture.detectChanges();
    expect(mockApiService.getTeamsList).toHaveBeenCalled();
    expect(component.teams).toEqual(mockTeams);
  });

  it('should update teams and emit filter change', (done) => {
    fixture.detectChanges();
    spyOn(component.filterChange, 'emit');
    component.onTeamsSelected([1, 2]);
    setTimeout(() => {
      expect(component.selectedTeamIds).toEqual([1, 2]);
      expect(component.filterChange.emit).toHaveBeenCalled();
      done();
    }, 0);
  });

  it('should update positions and emit filter change', (done) => {
    fixture.detectChanges();
    spyOn(component.filterChange, 'emit');
    component.onPositionsSelected([1]);
    setTimeout(() => {
      expect(component.selectedPositions).toEqual([1]);
      expect(component.filterChange.emit).toHaveBeenCalled();
      done();
    }, 0);
  });

  it('should update min minutes', () => {
    fixture.detectChanges();
    const event = { target: { value: '500' } };
    component.onMinMinutesBlur(event);
    expect(component.minMinutes).toBe(500);
  });

  it('should handle invalid min minutes input', () => {
    fixture.detectChanges();
    const event = { target: { value: 'invalid' } };
    component.onMinMinutesBlur(event);
    expect(component.minMinutes).toBeNull();
  });

  it('should update player name and emit filter change', (done) => {
    fixture.detectChanges();
    spyOn(component.filterChange, 'emit');
    const event = { target: { value: 'Salah' } };
    component.onPlayerNameChange(event);
    setTimeout(() => {
      expect(component.playerName).toBe('Salah');
      expect(component.filterChange.emit).toHaveBeenCalled();
      done();
    }, 0);
  });

  it('should reset all filters', () => {
    fixture.detectChanges();
    component.selectedTeamIds = [1];
    component.selectedPositions = [2];
    component.minMinutes = 500;
    component.playerName = 'Test';
    
    component.onResetFilters();
    
    expect(component.selectedTeamIds).toEqual([]);
    expect(component.selectedPositions).toEqual([]);
    expect(component.minMinutes).toBeNull();
    expect(component.playerName).toBe('');
  });

  it('should not emit duplicate filter changes', (done) => {
    fixture.detectChanges();
    component.selectedTeamIds = [1];
    component['emitChange']();
    const emitSpy = spyOn(component.filterChange, 'emit');
    component['emitChange']();
    setTimeout(() => {
      expect(emitSpy).not.toHaveBeenCalled();
      done();
    }, 0);
  });
});
