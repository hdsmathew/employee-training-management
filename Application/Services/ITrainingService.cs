﻿using Core.Application.Models;
using Core.Domain;

namespace Core.Application.Services
{
    public interface ITrainingService
    {
        ResponseModel<Training> Add(Training training);
        ResponseModel<Training> Update(Training training);
        ResponseModel<Training> Delete(short trainingID);
        ResponseModel<TrainingViewModel> GetTrainingDetails();
        ResponseModel<TrainingViewModel> GetTrainingDetails(short trainingId);
    }
}
